import { Component, OnInit, ViewChild } from '@angular/core';
import { ContentViewModel } from '../../../models/content/content-view-model.model';
import {DataService } from '../../../services/data.service';
import { AlertService, MessageSeverity } from '../../../services/alert.service';

@Component({
  selector: 'content-management',
  templateUrl: './contentmanagement.component1.html',
  styleUrls: ['./contentmanagement.component.css']
})
export class ContentmanagementComponent implements OnInit {
  constructor(private dataService: DataService,private alertService : AlertService) {
    debugger;
    this.loadAllContent(0);
    
    
  }

  dropdownSettings: any;
  name = 'ng2-ckeditor';
  ckeConfig: any;
  mycontent: string;
  log: string = '';
  @ViewChild("myckeditor") ckeditor: any;
  allContent: ContentViewModel[];
  selectedContent: ContentViewModel = new ContentViewModel();
  defaultContent: ContentViewModel = new ContentViewModel(-1, "Select Content Title", "");
  displayNewContentTitle: boolean = false;

  ngOnInit() {
    this.ckeConfig = {
      allowedContent: false,
      extraPlugins: 'divarea',
      forcePasteAsPlainText: true
    };
  }

  addNewContent() {
    let newContent = new ContentViewModel();
    this.allContent.push(newContent);
    this.selectedContent = newContent;
    //this.displayNewContentTitle = true;
  }

  cancelNewContent() {
    if (this.displayNewContentTitle) {

    }
  }

  onContetChange(event) {
    debugger;
    var content = this.selectedContent;
    debugger;
  }

  loadAllContent(contentId : number) {
    this.alertService.startLoadingMessage('Loading content data.....');
    this.dataService.get<ContentViewModel[]>('api/Content/GetContent')
      .subscribe((data) => {
        debugger;
        this.allContent = data;
        if (contentId == 0) {
          this.selectedContent = this.allContent[0];
        }
        else {
          this.selectedContent = this.allContent.find(c => c.ContentId == contentId);
        }
        this.alertService.stopLoadingMessage();
      });
  }

  saveContent() {
    this.dataService.post<number>('api/Content/SaveContent', this.selectedContent)
      .subscribe((contentId) => {
        if (contentId) {
          this.alertService.showMessage("Content",'Content saved successfully!!',MessageSeverity.success);
          this.loadAllContent(contentId);
          
          //if (this.selectedContent.ContentId > 0) {
          //  let content = this.allContent.find(c => c.ContentId == this.selectedContent.ContentId);
          //  this.selectedContent = content;
          //}
          //else {
          //  this.selectedContent = 
          //}
          
          }
      });
  }

}
