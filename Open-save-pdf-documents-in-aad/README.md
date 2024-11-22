# Open and Save PDF Documents in Azure AD

This repository contains examples to open and save PDF documents in Azure Active Directory (Azure AD) using the Syncfusion PDF Viewer.

## Prerequisites

To run this application, you need the following:

- **Visual Studio 2019** or later versions.
- An **Azure Storage Account** and **Azure Active Directory (AAD)** credentials.

## Setting up Azure AD Credentials

In order to connect to Azure Blob Storage using Azure AD, you need to provide your Azure AD credentials. 

Open the `PdfViewerController.cs` file in the **Controllers** folder and update the following fields with your Azure AD credentials:

```csharp
string tenantId = "Provide your tenant ID here";
string clientId = "Provide your client ID here";
string clientSecret = "Provide your client secret here";
string blobServiceEndpoint = "https://your-storage-account.blob.core.windows.net";
string containerName = "Provide your container name here";
```

## Deploying and Running the Sample

**Step 1** To deploy and run the webservice sample, follow these steps:

**Step 2** Open Visual Studio 2019 or a later version.

**Step 3** Open the solution file (.sln) in Visual Studio.

**Step 4** Build the solution to ensure all the dependencies are resolved.

**Step 5** To run the sample in debugging mode, press F5 or select Debug > Start Debugging from the menu. This will launch the webservice and allow you to debug the application.


# Running the AngularClient Sample

To run the AngularClient sample of the PDF Viewer component, follow these steps

**Step 1** Open a terminal or command prompt.

**Step 2**  Navigate to the root directory of the AngularClient sample.

**Step 3**  Run the following command to install the necessary dependencies

```
npm install
```

This command will download and install all the required packages and dependencies specified in the `package.json` file.

**Step 4**  Once the installation is complete, run the following command to start the Angular development server and open the PDF Viewer component in the browser:

```
ng serve --open
```
The `ng serve` command compiles the Angular application and starts a development server. The `--open` flag automatically opens the application in your default browser.

After executing these steps, the PDF Viewer component should be displayed in your browser, allowing you to view and interact with PDF documents stored in Azure Blob Storage.

# Running the ReactClient Sample

To run the ReactClient sample of the PDF Viewer component, follow these steps

**Step 1** Open a terminal or command prompt.

**Step 2** Navigate to the root directory of the ReactClient sample.

**Step 3** Run the following command to install the necessary dependencies

```
npm install
```

This command will download and install all the required packages and dependencies specified in the `package.json` file.

**Step 4** Once the installation is complete, run the following command to start the React development server and open the PDF Viewer component in the browser

```
npm start
```

The `npm start` command starts the React development server and automatically opens the application in your default browser.

After executing these steps, the PDF Viewer component should be displayed in your browser, allowing you to view and interact with PDF documents stored in Azure Blob Storage.

# Running the JavaScriptClient Sample

To run the JavaScriptClient sample of the PDF Viewer component, follow these steps

**Step 1** Open a terminal or command prompt.

**Step 2** Navigate to the root directory of the JavaScriptClient sample.

**Step 3** Run the following command to install the necessary dependencies

```
npm install
```

This command will download and install all the required packages and dependencies specified in the `package.json` file.

**Step 4** Once the installation is complete, run the following command to start the JavaScript development server and open the PDF Viewer component in the browser

```
npm start
```

The `npm start` command starts the JavaScript development server and automatically opens the application in your default browser.

After executing these steps, the PDF Viewer component should be displayed in your browser, allowing you to view and interact with PDF documents stored in Azure Blob Storage.

# Running the VueClient Sample

To run the VueClient sample of the PDF Viewer component, follow these steps

**Step 1** Open a terminal or command prompt.

**Step 2** Navigate to the root directory of the VueClient sample.

**Step 3** Run the following command to install the necessary dependencies

```
npm install
```

This command will download and install all the required packages and dependencies specified in the `package.json` file.

**Step 4** Once the installation is complete, run the following command to start the Vue development server and open the PDF Viewer component in the browser

```
npm run dev  
```

The `npm run dev` command starts the Vue development server and automatically opens the application in your default browser.

After executing these steps, the PDF Viewer component should be displayed in your browser, allowing you to view and interact with PDF documents stored in Azure Blob Storage.

# Running the TypeScriptClient Sample

To run the TypeScriptClient sample of the PDF Viewer component, follow these steps

**Step 1** Open a terminal or command prompt.

**Step 2** Navigate to the root directory of the TypeScriptClient sample.

**Step 3** Run the following command to install the necessary dependencies

```
npm install
```

This command will download and install all the required packages and dependencies specified in the `package.json` file.

**Step 4** Once the installation is complete, run the following command to start the TypeScript development server and open the PDF Viewer component in the browser

```
npm start
```

The `npm start` command starts the TypeScript development server and automatically opens the application in your default browser.

After executing these steps, the PDF Viewer component should be displayed in your browser, allowing you to view and interact with PDF documents stored in Azure Blob Storage.
