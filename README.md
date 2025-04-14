# Resource Editor CLI

A .NET command-line tool for managing binary and text resources stored within a SQLite database.

## Prerequisites

*   .NET 8 SDK or later

## Building

1.  Clone the repository.
2.  Navigate to the solution directory (`ResourceEditorCli`).
3.  Run the build command:
    ```bash
    dotnet build
    ```
    The executable will typically be found in `ResourceEditorCli/bin/Debug/net8.0/ResourceEditorCli.exe`.

## Usage

The tool uses a verb-based command structure. The general format is:

```bash
ResourceEditorCli <verb> [options]
```

**Important:** Many commands operate on the "currently set" database. This path is stored in the `appsettings.json` file within the application's directory. Use the `setdb` command to configure this.

### Commands

*   **`createdb <path>`**
    *   Creates a new, empty SQLite resource database file at the specified `<path>`.
    *   Example: `ResourceEditorCli createdb myResources.db`

*   **`setdb <path>`**
    *   Sets the database file at `<path>` as the active database for subsequent commands. Stores the path in `appsettings.json`.
    *   The file must exist.
    *   Example: `ResourceEditorCli setdb C:\data\myResources.db`

*   **`infodb [path]`**
    *   Displays metadata (Version, Date Created) about a resource database.
    *   If `<path>` is provided, it reads that specific file.
    *   If `<path>` is omitted, it reads the currently set database (from `appsettings.json`).
    *   Example (current DB): `ResourceEditorCli infodb`
    *   Example (specific DB): `ResourceEditorCli infodb anotherResources.db`

*   **`addresource [options]`**
    *   Adds a resource to the currently set database.
    *   Requires specifying the resource type (`-b` or `-t`) and the source file (`-r`).
    *   **Options:**
        *   `-b`, `--binary`: Specify that the resource is binary data.
        *   `-t`, `--text`: Specify that the resource is text data.
        *   `-s`, `--namespace <namespace>`: (Optional) The namespace for the resource (default: "root").
        *   `-n`, `--name <name>`: (Required) The unique name for the resource within its namespace.
        *   `-r`, `--resourcefilename <filepath>`: (Required) Path to the file containing the resource data to add.
        *   `-e`, `--extendedattfilename <filepath>`: (Optional) Path to a text file containing extended attributes to store alongside the resource.
    *   Example: `ResourceEditorCli addresource -b -s images -n logo -r ./logo.png`
    *   Example: `ResourceEditorCli addresource -t -n config -r ./settings.json -e ./metadata.txt`

*   **`listresources [options]`**
    *   Lists resources stored in the currently set database.
    *   Requires specifying a filter type (`-a`, `-b`, or `-t`).
    *   **Options:**
        *   `-b`, `--binary`: List only binary resources.
        *   `-t`, `--text`: List only text resources.
        *   `-a`, `--all`: List all resources.
        *   `-s`, `--namespace <namespace>`: (Optional) Filter the list by namespace.
        *   `-n`, `--name <name>`: (Optional) Filter the list by resource name.
    *   Example: `ResourceEditorCli listresources -a`
    *   Example: `ResourceEditorCli listresources -b -s images`

*   **`getresourcebyid [options]`**
    *   Retrieves a specific resource from the currently set database by its unique ID and saves it to a file.
    *   **Options:**
        *   `-i`, `--id <guid>`: (Required) The unique GUID identifier of the resource to retrieve.
        *   `-f`, `--filename <filepath>`: (Optional) The path where the retrieved resource data should be saved. If omitted, it uses the original filename stored in the database header.
    *   Example: `ResourceEditorCli getresourcebyid -i "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" -f ./retrieved_logo.png`

*   **`deleteresourcebyid [options]`**
    *   Deletes a specific resource from the currently set database by its unique ID.
    *   **Options:**
        *   `-i`, `--id <guid>`: (Required) The unique GUID identifier of the resource to delete.
        *   `-v`, `--shrink`: (Optional) Shrink the database file after deleting the resource.
    *   Example: `ResourceEditorCli deleteresourcebyid -i "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"`
    *   Example (with shrink): `ResourceEditorCli deleteresourcebyid -i "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" -v`

*   **`deleteresourcebyname [options]`**
    *   Deletes a specific resource from the currently set database by its type, namespace, and name.
    *   Requires specifying the resource type (`-b` or `-t`) and the name (`-n`).
    *   **Options:**
        *   `-b`, `--binary`: Specify binary resource type.
        *   `-t`, `--text`: Specify text resource type.
        *   `-s`, `--namespace <namespace>`: (Optional) The namespace of the resource (default: "root").
        *   `-n`, `--name <name>`: (Required) The name of the resource.
        *   `-v`, `--shrink`: (Optional) Shrink the database file after deleting the resource.
    *   Example: `ResourceEditorCli deleteresourcebyname -b -s images -n old_logo`
    *   Example (with shrink): `ResourceEditorCli deleteresourcebyname -t -s config -n user_settings -v`

*   **`shrink`**
    *   Performs a VACUUM operation on the currently set SQLite database to potentially reduce its file size.
    *   Example: `ResourceEditorCli shrink`

## Examples

1.  **Create a new database and set it as active:**
    ```bash
    ResourceEditorCli createdb project.resdb
    ResourceEditorCli setdb project.resdb
    ```

2.  **Add an image and a config file:**
    ```bash
    ResourceEditorCli addresource -b -s assets/images -n main_icon -r ./icons/app_icon.ico
    ResourceEditorCli addresource -t -s config -n user_settings -r ./defaults/settings.xml
    ```

3.  **List all resources:**
    ```bash
    ResourceEditorCli listresources -a
    ```

4.  **Get the icon back out:**
    ```bash
    # First, list to find the ID
    ResourceEditorCli listresources -b -s assets/images -n main_icon
    # (Copy the ID from the output)
    ResourceEditorCli getresourcebyid -i "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" -f ./extracted_icon.ico
    ```

5.  **Delete the settings file by name:**
    ```bash
    ResourceEditorCli deleteresourcebyname -t -s config -n user_settings
    ```

6.  **Delete an image by ID and shrink the database:**
    ```bash
    # First, list to find the ID
    ResourceEditorCli listresources -b -s assets/images -n main_icon
    # (Copy the ID from the output)
    ResourceEditorCli deleteresourcebyid -i "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" -v
    ```