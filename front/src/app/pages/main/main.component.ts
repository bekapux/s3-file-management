import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { S3Service } from 'src/app/_services/S3.Service/s3.service';
import {
  DirectoryItemModelDto,
  ObjectType,
} from 'src/app/_services/S3.Service/models/DirectoryItemModeldto';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { BehaviorSubject, firstValueFrom } from 'rxjs';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
})
export class MainComponent implements OnInit {
  breadCrumbs: string[] = [];
  termToSearch$: BehaviorSubject<string> = new BehaviorSubject('');
  objects: DirectoryItemModelDto[] = [];
  destroyRef = inject(DestroyRef);
  randomNumbers: number[] = [];

  constructor(private s3Service: S3Service) {
    this.generatePlaceholderNumbers();
  }

  ngOnInit(): void {
    this.termToSearch$
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe((x) => {
        firstValueFrom(this.s3Service.getFileNames(x)).then((data) => {
          this.objects = data;
        });
      });
  }

  public onObjectChosen(displayName: string, objectType: ObjectType) {
    if (objectType === ObjectType.Folder) {
      this.objects = [];
      this.generatePlaceholderNumbers();
      this.breadCrumbs.push(displayName);
      this.termToSearch$.next(this.breadCrumbs.join('/'));
    }
  }

  public onBreadcrumbClick(index: number) {
    if(this.breadCrumbs.length - 1 === index) return;
    this.generatePlaceholderNumbers();
    this.objects = [];
    this.breadCrumbs.splice(index + 1);
    this.termToSearch$.next(this.breadCrumbs.join('/'));
  }

  public onDelete(objectKey: string) {
    console.log(objectKey);
  }

  private generatePlaceholderNumbers() {
    this.randomNumbers = [];
    for (let i = 0; i < this.generateRandomNumber(1, 7); i++) {
      this.randomNumbers.push(this.generateRandomNumber(1, 8));
    }
  }

  private generateRandomNumber(min: number, max: number): number {
    return Math.floor(Math.random() * (max - min + 1)) + min;
  }
}
