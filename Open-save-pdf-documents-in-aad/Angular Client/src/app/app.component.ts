import { Component, OnInit } from '@angular/core';
import {
  LinkAnnotationService, BookmarkViewService, MagnificationService,
  ThumbnailViewService, ToolbarService, NavigationService,
  AnnotationService, TextSearchService, TextSelectionService,
  PrintService, FormDesignerService, FormFieldsService, CustomToolbarItemModel
} from '@syncfusion/ej2-angular-pdfviewer';
import { ComboBox } from "@syncfusion/ej2-dropdowns";
import { TextBox } from "@syncfusion/ej2-inputs";

@Component({
  selector: 'app-root',
  template: `<div class="content-wrapper">
                <ejs-pdfviewer id="pdfViewer"
                      [documentPath]="document"
                      [serviceUrl]="service"
                      [toolbarSettings]="toolbarSettings"
                      (toolbarClick)="toolbarClick($event)"
                      style="height:640px;display:block">
                </ejs-pdfviewer>
             </div>`,
  providers: [
    LinkAnnotationService, BookmarkViewService, MagnificationService,
    ThumbnailViewService, ToolbarService, NavigationService,
    AnnotationService, TextSearchService, TextSelectionService,
    PrintService, FormDesignerService, FormFieldsService
  ]
})
export class AppComponent implements OnInit {
  public document: string = ''; 
  public service: string = 'https://localhost:44308/pdfviewer';
  // Custom Toolbar Items
  public toolItem1: CustomToolbarItemModel = {
    id: 'loadFromAAD',
    text: 'Load From AAD',
    tooltipText: 'Custom toolbar item',
    align: 'left'
  };

  public toolItem2: CustomToolbarItemModel = {
    id: 'saveToAAD',
    text: 'Save To AAD',
    tooltipText: 'Custom toolbar item',
    align: 'left'
  };


  // Toolbar settings
  public toolbarSettings = {
    showTooltip: true,
    toolbarItems: [
      this.toolItem1, 
      this.toolItem2, 
      'OpenOption', 
      'PageNavigationTool', 
      'MagnificationTool', 
      'PanTool', 
      'SelectionTool', 
      'SearchOption', 
      'PrintOption', 
      'DownloadOption', 
      'UndoRedoTool', 
      'AnnotationEditTool', 
      'FormDesignerEditTool', 
      'CommentTool', 
      'SubmitForm'
    ]
  };

  constructor() { }

  ngOnInit(): void {
  }

  // Toolbar click event handler
  public toolbarClick(args: any): void {
    var viewer = (<any>document.getElementById('pdfViewer')).ej2_instances[0];
    if (args.item && args.item.id === 'loadFromAAD') {
      var xhr = new XMLHttpRequest();
      xhr.open(
        'POST',
        `https://localhost:44308/pdfviewer/LoadFromAAD?fileName=pdf-succinctly.pdf`,
        true
      );
      xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200) {
          var data = xhr.responseText;
          console.log(data); // Handle the  response
          viewer.load(data);
        }
      };
      xhr.send();
    } else if (args.item && args.item.id === 'saveToAAD') {
        viewer.serverActionSettings.download = "SaveToAAD";
      viewer.download();
    }
  }
}
