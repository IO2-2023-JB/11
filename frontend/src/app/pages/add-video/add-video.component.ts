import { Component, OnDestroy, ViewChild } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { FileUpload } from 'primeng/fileupload';
import { Observable, Subscription, finalize, of, switchMap, tap } from 'rxjs';
import { VideoMedatadataDTO } from './models/video-metadata-dto';
import { AddVideoService } from './services/add-video.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-add-video',
  templateUrl: './add-video.component.html',
  styleUrls: ['./add-video.component.scss']
})
export class AddVideoComponent implements OnDestroy {
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
  video: FormData | null = null;
  videoUploadTouched = false;
  supportedVideoTypes = ['video/mp4', 'video/avi', 'video/webm', 'video/x-matroska'];
  @ViewChild('thumbnailUpload') thumbnailUpload!: FileUpload;
  @ViewChild('videoUpload') videoUpload!: FileUpload;

  constructor(private addVideoService: AddVideoService, private messageService: MessageService) { }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => {
      subscription.unsubscribe();
    });
  }

  private doWithLoading(observable$: Observable<any>): Observable<any> {
    return of(this.isProgressSpinnerVisible = true).pipe(
      switchMap(() => observable$),
      finalize(() => this.isProgressSpinnerVisible = false)
    );
  }

  onSubmit(): void {
    if (this.addVideoForm.invalid || this.video === null) {
      this.addVideoForm.markAllAsTouched();
      this.videoUploadTouched = true;
      return;
    }

    const videoMedatadaDTO: VideoMedatadataDTO = {
      title: this.addVideoForm.get('title')!.value!,
      description: this.addVideoForm.get('description')!.value!,
      thumbnail: this.addVideoForm.get('thumbnail')!.value!,
      tags: this.addVideoForm.get('tags')!.value!,
      visibility: this.addVideoForm.get('visibility')!.value!
    };

    const uploadVideo$ = this.addVideoService.postVideoMetadata(videoMedatadaDTO).pipe(
      switchMap(videoId => {
        return this.addVideoService.uploadVideo(videoId, this.video!).pipe(
          tap(() => {
            this.messageService.add({
              severity: 'success',
              summary: 'Success',
              detail: 'Video upload accepted'
            })
          }),
        )
      }),
    );
    this.subscriptions.push(this.doWithLoading(uploadVideo$).subscribe());
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

  handleOnThumbnailSelect(event: { originalEvent: Event; files: File[] }): void {
    const thumbnailFile = event.files[0];
    if (thumbnailFile.type === 'image/png' || thumbnailFile.type === 'image/jpeg') {
      const reader = new FileReader();
      reader.readAsDataURL(thumbnailFile);
      reader.onload = () => {
        this.addVideoForm.patchValue({thumbnail: reader.result as string});
      };
    }
  }

  handleOnThumbnailRemove(): void {
    this.addVideoForm.patchValue({thumbnail: ''});
    this.thumbnailUpload.clear();
  }

  handleOnVideoSelect(event: { originalEvent: Event; files: File[] }): void {
    this.videoUploadTouched = true;
    const uploadedVideo = event.files[0];
    if (this.supportedVideoTypes.includes(uploadedVideo.type)) {
      this.video = new FormData();
      this.video.append('videoFile', new Blob([uploadedVideo], {type: uploadedVideo.type}));
    }
  }

  handleOnVideoRemove(): void {
    this.video = null;
    this.videoUpload.clear();
    this.videoUploadTouched = true;
  }

  isVideoInputInvalidAndTouchedOrDirty(): boolean {
    return this.videoUploadTouched && this.video === null;
  }
}
