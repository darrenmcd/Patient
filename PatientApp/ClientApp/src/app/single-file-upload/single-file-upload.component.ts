import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { throwError } from "rxjs";

@Component({
  selector: "app-single-file-upload",
  templateUrl: "./single-file-upload.component.html",
  styleUrls: ["./single-file-upload.component.css"],
})
export class SingleFileUploadComponent  implements OnInit {
  status: "initial" | "uploading" | "success" | "fail" = "initial"; // Variable to store file status
  file: File | null = null; // Variable to store file

  private url!: string;

  constructor(private http: HttpClient,  @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl + 'file';
  }

  ngOnInit(): void {}

  // On file Select
  onChange(event: any) {
    const file: File = event.target.files[0];

    if (file) {
      this.status = "initial";
      this.file = file;
    }
  }

  onUpload() {
    if (this.file) {
      const formData = new FormData();

      formData.append("file", this.file, this.file.name);

      const upload$ = this.http.post(this.url + "/upload", formData);

      this.status = "uploading";

      upload$.subscribe({
        next: () => {
          this.status = "success";
        },
        error: (error: any) => {
          this.status = "fail";
          return throwError(() => error);
        },
      });
    }
  }
}
