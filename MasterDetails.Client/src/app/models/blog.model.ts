import { Author } from './author.model';
import { Tag } from './tag.model';
import { BlogVideo } from './blog-video.model';

export interface Blog {
  blogID: number;
  title: string;
  content: string;
  coverImageUrl: string;
  createdAt: Date; 
  isPublished: boolean;

  authorName: string; 
  tagNames: string[];
  blogVideos: BlogVideo[];
}
