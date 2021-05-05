# Xero Blazor Files App 

This project is a Blazor WebAssembly Demo Application demonstrating the use of the Xero Files API, using the Xero NetStandard SDK to upload and delete files, folders and associations.

## Features

- Upload File, Upload File to a Folder, Delete file
- Create Folder, Delete Folder
- Create Association, Get Associations for a File

## Running the Application
### Create a Xero app for OAuth 2.0
You will need to create a Xero App on the developer portal to run the application.
If you don’t have a Xero user account, you can create one for free. Once you’ve activated your user account and enabled the Demo company:
Login to Xero developer portal and click “New app”.
Enter your app name, company url, privacy policy url, and redirect URI (https://localhost:5001/authentication/login-callback).
Make sure you select “Auth code with PKCE” option as it provides better security for a Single Page Application
Agree to the terms and conditions and click “Create App”.
Click the “Save” button. Your secret is now hidden.

### Configure your Xero Client ID

Open the appsettings file in the Client directory (BlazorFilesApp/Client/wwwroot/appsettings.json)

Replace "YOUR_CLIENT_ID" with the Client ID you saved during your Xero App setup.

### Run the Server project
Open a terminal in VS Code and navigate to the Server directory (BlazorFilesApp/Server)

Use the ```dotnet run``` command to start the application.

## License
This software is published under the MIT License.
```
Copyright (c) 2020 Xero Limited

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
```

