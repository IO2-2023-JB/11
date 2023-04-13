import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
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
  @ViewChild('thumbnailUpload') thumbnailUpload!: FileUpload;

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

    const postVideoMetadata$ = this.addVideoService.postVideoMetadata(videoMedatadaDTO).pipe(
      tap(() => {
        this.messageService.add({
          severity: 'success',
          summary: 'Success',
          detail: 'Video metadata added'
        })
      }),
    );
    this.subscriptions.push(this.doWithLoading(postVideoMetadata$).subscribe());
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
