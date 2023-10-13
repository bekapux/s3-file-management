export interface DirectoryItemModelDto {
  key: string;
  displayName: string;
  type: ObjectType;
}

export enum ObjectType {
  File,
  Folder,
}
