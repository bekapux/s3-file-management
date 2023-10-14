export interface DirectoryItemModelDto {
  key: string;
  displayName: string;
  type: ObjectType;
  downloadUrl?: string
}

export enum ObjectType {
  File,
  Folder,
}
