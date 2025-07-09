import { Component, EventEmitter, Input, Output } from '@angular/core';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { MatChipInputEvent } from '@angular/material/chips';

import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { Tag } from 'src/app/models/tag.model';
import { TagService } from 'src/app/features/blog/core/services/tag.service';

@Component({
  selector: 'app-tag-input',
  templateUrl: './tag-input.component.html',
  styleUrls: ['./tag-input.component.scss']
})
export class TagInputComponent {
  @Input() label: string = 'Tags';
  @Input() availableTags: Tag[] = [];
  @Input() maxTags: number = 7;
  @Input() initialTags: string[] = [];

  @Output() tagListChange = new EventEmitter<string[]>();

  tagNames: string[] = [];
  readonly separatorKeysCodes = [ENTER, COMMA] as const;

  constructor(private tagService: TagService, private snackbar: SnackbarService) {}

  ngOnInit(): void {
    this.tagNames = [...this.initialTags];
  }

  addTag(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value?.trim();

    if (!value) return;

    if (this.tagNames.length >= this.maxTags) {
      this.snackbar.show(`⚠️ Maximum ${this.maxTags} tags allowed.`, 'Close');
    } else if (!this.tagNames.includes(value)) {
      this.tagNames.push(value);
      this.tagListChange.emit(this.tagNames);

      // Save to tag table if not already available
      const exists = this.availableTags.find(t => t.name.toLowerCase() === value.toLowerCase());
      if (!exists) {
        this.tagService.create({ name: value }).subscribe({
          next: (created) => this.availableTags.push(created),
          error: () => this.snackbar.show('❌ Failed to save new tag.', 'Close')
        });
      }
    }

    if (input) input.value = '';
  }

  removeTag(index: number): void {
    this.tagNames.splice(index, 1);
    this.tagListChange.emit(this.tagNames);
  }
}
