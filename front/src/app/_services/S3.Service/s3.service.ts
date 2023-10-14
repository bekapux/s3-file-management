import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { DirectoryItemModelDto } from './models/DirectoryItemModeldto';

@Injectable({
  providedIn: 'root',
})
export class S3Service {
  constructor(private http: HttpClient) {}

  public getFileNames(key: string = ''): Observable<DirectoryItemModelDto[]> {
    return this.http.get<DirectoryItemModelDto[]>(
      `${environment.webApi}/get${key ? `?directoryPath=${key}` : ''}`
    );

    return of([
      {
        key: '',
        displayName: 'MyFiles',
        type: 1,
      },
      {
        key: '',
        displayName: 'other',
        type: 1,
      },
      {
        key: '/iconmonstr-folder-thin.svg',
        displayName: 'iconmonstr-folder-thin.svg',
        type: 0,
      },
    ]);
  }

  public deleteFile(fileKey: string) {
    return this.http.delete(`${environment.webApi}/delete/${fileKey}`);
  }
}
