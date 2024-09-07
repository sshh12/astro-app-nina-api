import re
import json

WEB_MANAGER_PATH = "AstroAppHTTPAPI\Web\WebServerManager.cs"
ROUTE_PATH = "AstroAppHTTPAPI\Web\Routes\{controller}.cs"
OUTPUT_PATH = "API.md"

def extract_routes_and_controllers(snippet):
    # Define the regex pattern to match the routes and controllers
    pattern = re.compile(r'\.WithWebApi\("(/api/v1/[^"]+)", m => m\.WithController\(\(\) => new ([^()]+)\(')
    
    # Find all matches in the snippet
    matches = pattern.findall(snippet)
    
    # Create a dictionary to store the mappings
    route_controller_mapping = {route: controller for route, controller in matches}
    
    return route_controller_mapping

def _find_matching_brace(code, start_index):
    stack = []
    for i in range(start_index, len(code)):
        if code[i] == '{':
            stack.append('{')
        elif code[i] == '}':
            stack.pop()
            if not stack:
                return i
    return -1

def extract_types(csharp_code):
    # Define regex patterns to match class definitions
    class_pattern = re.compile(r'public class (\w+)\s*{')
    property_pattern = re.compile(r'public (\w+(\?)?) (\w+) { get; set; }')

    classes = {}

    # Find all class definitions
    for class_match in class_pattern.finditer(csharp_code):
        class_name = class_match.group(1)
        class_start = class_match.end()
        class_end = _find_matching_brace(csharp_code, class_start - 1)
        class_body = csharp_code[class_start:class_end]

        properties = {}

        # Find all properties within the class
        for prop_match in property_pattern.finditer(class_body):
            prop_type = prop_match.group(1)
            prop_name = prop_match.group(3)
            properties[prop_name] = prop_type

        classes[class_name] = properties

    return classes

def parse_endpoints(csharp_code):
    # Define regex patterns to match route definitions
    route_pattern = re.compile(r'\[Route\(HttpVerbs\.(\w+), "(/[^"]*)"\)\]\s*public async Task (\w+)\(\[JsonData\] (\w+) (\w+)\)')
    route_pattern_no_data = re.compile(r'\[Route\(HttpVerbs\.(\w+), "(/[^"]*)"\)\]\s*public async (Task|void) (\w+)\(\)')
    route_pattern_get_with_type = re.compile(r'\[Route\(HttpVerbs\.(\w+), "(/[^"]*)"\)\]\s*public async Task (\w+)\((\w+) (\w+)\)')

    endpoints = {}

    # Find all route definitions with request data
    for match in route_pattern.finditer(csharp_code):
        method = match.group(1).upper()
        endpoint = match.group(2)
        request_type = match.group(4)
        endpoints[endpoint] = {"method": method, "type": request_type}

    # Find all route definitions without request data
    for match in route_pattern_no_data.finditer(csharp_code):
        method = match.group(1).upper()
        endpoint = match.group(2)
        endpoints[endpoint] = {"method": method, "type": None}

    # Find all GET route definitions with a different type
    for match in route_pattern_get_with_type.finditer(csharp_code):
        method = match.group(1).upper()
        endpoint = match.group(2)
        request_type = match.group(3)
        endpoints[endpoint] = {"method": method, "type": request_type}

    return dict(sorted(endpoints.items()))

def generate_rest_markdown(title, prefix, endpoints, types):
    markdown = f"#### {title} Endpoints\n\n"
    response_types = list({k: v for k, v in types.items() if k.endswith("StatusResponse")}.values())[0]
    response_types_text = json.dumps({k: '...' for k in response_types})
    for endpoint, details in endpoints.items():
        method = details["method"]
        request_type = details["type"]
        markdown += f"<details>\n <summary><code>{method}</code> <code><b>{prefix + endpoint}</b></code></summary>\n\n"
        markdown += "##### Parameters\n\n"
        if request_type:
            markdown += "> | name      |  type     | data type               | description                                                           |\n"
            markdown += "> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|\n"
            for prop_name, prop_type in types.get(request_type, {}).items():
                markdown += f"> | {prop_name} | required | {prop_type} | N/A  |\n"
        else:
            markdown += "> None\n"
        markdown += "\n##### Responses\n\n"
        markdown += "> | http code     | content-type                      | response                                                            |\n"
        markdown += "> |---------------|-----------------------------------|---------------------------------------------------------------------|\n"
        markdown += f"> | `200`         | `application/json`                | `{response_types_text}`                                             |\n"
        markdown += "</details>\n\n"
        markdown += "------------------------------------------------------------------------------------------\n\n"
    return markdown

def generate_events_markdown(title, prefix, types):
    markdown = f"#### {title} Events\n\n"
    status_response_types = {k: v for k, v in types.items() if k.endswith("StatusResponse")}
    for name, types in status_response_types.items():
        markdown += f"<details>\n <summary><code>Event</code> <code><b>{name}</b></code></summary>\n\n"
        markdown += "##### Data\n\n"
        markdown += "> | name      |  type     | data type               | description                                                           |\n"
        markdown += "> |-----------|-----------|-------------------------|-----------------------------------------------------------------------|\n"
        for prop_name, prop_type in types.items():
            markdown += f"> | {prop_name} | required | {prop_type} | N/A  |\n"
        markdown += "</details>\n\n"
        markdown += "------------------------------------------------------------------------------------------\n\n"
    return markdown

def main():
    with open(WEB_MANAGER_PATH, "r") as file:
        web_manager = file.read()
    route_to_controller = extract_routes_and_controllers(web_manager)
    with open(OUTPUT_PATH, "w") as f:
        f.write("## REST API Documentation\n\n")
        for route, controller in route_to_controller.items():
            with open(ROUTE_PATH.format(controller=controller), "r") as file:
                route_file = file.read()
            route_types = extract_types(route_file)
            endpoints = parse_endpoints(route_file)
            title = controller.replace("RouteController", "")
            title = re.sub(r"(\w)([A-Z])", r"\1 \2", title)
            markdown = generate_rest_markdown(title, route, endpoints, route_types)
            f.write(markdown)
        f.write("## Websocket Events Documentation\n\n")
        for route, controller in route_to_controller.items():
            with open(ROUTE_PATH.format(controller=controller), "r") as file:
                route_file = file.read()
            route_types = extract_types(route_file)
            title = controller.replace("RouteController", "")
            title = re.sub(r"(\w)([A-Z])", r"\1 \2", title)
            markdown = generate_events_markdown(title, route, route_types)
            f.write(markdown)

if __name__ == "__main__":
    main()