import { Component } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { throwError } from "rxjs";

@Component({
  selector: "app-single-file-upload",
  templateUrl: "./single-file-upload.component.html",
  styleUrls: ["./single-file-upload.component.css"],
})
export class SingleFileUploadComponent {
  status: "initial" | "uploading" | "success" | "fail" = "initial"; // Variable to store file status
  file: File | null = null; // Variable to store file

  constructor(private http: HttpClient) {}

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

      const upload$ = this.http.post("localhost:50851/api/upload", formData);

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
