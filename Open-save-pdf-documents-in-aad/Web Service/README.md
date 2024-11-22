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


## Deploying and Running the Sample

**Step 1** To deploy and run the webservice sample, follow these steps:

**Step 2** Open Visual Studio 2019 or a later version.

**Step 3** Open the solution file (.sln) in Visual Studio.

**Step 4** Build the solution to ensure all the dependencies are resolved.

**Step 5** To run the sample in debugging mode, press F5 or select Debug > Start Debugging from the menu. This will launch the webservice and allow you to debug the application.

**Step 6** To run the sample without debugging, press Ctrl+F5 or select Debug > Start Without Debugging. This will start the webservice without attaching the debugger.

