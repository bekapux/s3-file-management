import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { S3Service } from 'src/app/_services/S3.Service/s3.service';
import { ObjectType } from 'src/app/_services/S3.Service/models/DirectoryItemModeldto';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
})
export class MainComponent {
  breadCrumbs: string[] = [];
  termToSearch: string = '';
  filesObservable = inject(S3Service).getFileNames(this.termToSearch);

  onObjectChosen(displayName: string, objectType: ObjectType) {
    if (objectType === ObjectType.Folder) {
      this.breadCrumbs.push(displayName);
      this.termToSearch = this.breadCrumbs.join('/');
    }
  }

  onBreadcrumbClick(index: number) {
    this.breadCrumbs.splice(index + 1);
    this.termToSearch = this.breadCrumbs.join('/');
  }

  onDelete() {
    console.log('Deleting');
  }
}
