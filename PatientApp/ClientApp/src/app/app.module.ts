import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AgGridModule } from 'ag-grid-angular';
import { SingleFileUploadComponent } from './single-file-upload/single-file-upload.component';

@NgModule({
  imports: [BrowserModule, FormsModule, AgGridModule, HttpClientModule],
  declarations: [AppComponent, SingleFileUploadComponent],
  bootstrap: [AppComponent],
})
export class AppModule {}
