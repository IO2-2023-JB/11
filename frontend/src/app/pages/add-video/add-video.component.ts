import { Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { FileUpload } from 'primeng/fileupload';
import { Subscription } from 'rxjs';
import { VideoMedatadataDTO } from './models/video-metadata-dto';

@Component({
  selector: 'app-add-video',
  templateUrl: './add-video.component.html',
  styleUrls: ['./add-video.component.scss']
})
export class AddVideoComponent implements OnInit {
  titleMaxLength = 100;
  descriptionMaxLength = 1000;
  visibilityOptions = ['Public', 'Private'];
  addVideoForm = new FormGroup({
    title: new FormControl('', [Validators.required, Validators.maxLength(this.titleMaxLength)]),
    description: new FormControl('', [Validators.required, Validators.maxLength(this.descriptionMaxLength)]),
    thumbnail: new FormControl('', Validators.required),
    tags: new FormControl([]),
    visibility: new FormControl(this.visibilityOptions[0], Validators.required),
  });
  subscriptions: Subscription[] = [];
  isProgressSpinnerVisible = false;
  @ViewChild('thumbnailUpload') thumbnailUpload!: FileUpload;

  constructor() { }

  ngOnInit(): void {
  }

  onSubmit(): void {
    if (this.addVideoForm.invalid) {
      this.addVideoForm.markAllAsTouched();
      return;
    }

    const videoMedatadaDTO: VideoMedatadataDTO = {
      title: this.addVideoForm.get('title')!.value!,
      description: this.addVideoForm.get('description')!.value!,
      thumbnail: this.addVideoForm.get('thumbnail')!.value!,
      tags: this.addVideoForm.get('tags')!.value!,
      visibility: this.addVideoForm.get('visibility')!.value!
    };
  }

  isInputInvalidAndTouchedOrDirty(inputName: string): boolean {
    const control = this.addVideoForm.get(inputName)!;
    return this.isInputTouchedOrDirty(control) && control.invalid;
  }

  isInputTouchedOrDirtyAndEmpty(inputName: string): boolean {
    const control = this.addVideoForm.get(inputName)!;
    return this.isInputTouchedOrDirty(control) && control.hasError('required');
  }

  isInputTouchedOrDirtyAndTooLong(inputName: string): boolean {
    const control = this.addVideoForm.get(inputName)!;
    return this.isInputTouchedOrDirty(control) && control.hasError('maxlength');
  }

  private isInputTouchedOrDirty(control: AbstractControl): boolean {
    return control.touched || control.dirty;
  }

  handleOnFileSelect(event: { originalEvent: Event; files: File[] }): void {
    const thumbnailFile = event.files[0];
    if (thumbnailFile.type === 'image/png' || thumbnailFile.type === 'image/jpeg') {
      const reader = new FileReader();
      reader.readAsDataURL(thumbnailFile);
      reader.onload = () => {
        this.addVideoForm.patchValue({thumbnail: reader.result as string});
      };
    }
  }

  handleOnRemove(): void {
    this.addVideoForm.patchValue({thumbnail: ''});
    this.thumbnailUpload.clear();
  }
}
