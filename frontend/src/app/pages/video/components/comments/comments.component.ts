import { Component } from '@angular/core';
import { Comment } from './models/comment';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss']
})
export class CommentsComponent {
  comments: Comment[] = [
    {
      id: '1',
      authorId: '1',
      content: 'comment content 1',
      avatarImage: 'https://filesdevelop.blob.core.windows.net/useravatars/431fbad7-2b0b-4232-ae7a-e3ba6af1b933',
      nickname: 'surfer',
      hasResponses: false,
      responses: null,
      isResponsesVisible: false,
    },
    {
      id: '2',
      authorId: '2',
      content: 'comment content 2',
      avatarImage: 'https://filesdevelop.blob.core.windows.net/useravatars/53_square.jpg',
      nickname: 'karol',
      hasResponses: false,
      responses: null,
      isResponsesVisible: false,
    },
  ];
  newComment = new FormControl('', Validators.required);

  isEmptyAndTouchOrDirty(formControl: FormControl): boolean {
    return formControl.invalid && (formControl.touched || formControl.dirty);
  }

  isTouchOrDirty(formControl: FormControl): boolean {
    return formControl.touched || formControl.dirty;
  }

  handleOnCancelCommentClick(formControl: FormControl): void {
    formControl.reset();
  }

  handleOnAddCommentClick(formControl: FormControl): void {
    // ADD LOGIC REGARDING ADDING COMMENTS
    formControl.reset();
  }

  isEmpty(formControl: FormControl): boolean {
    return formControl.value === '';
  }
}
