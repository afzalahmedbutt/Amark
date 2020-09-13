export class ContentViewModel {
  constructor(id = 0,title = "",description = "") {
    this.ContentId = id;
    this.Title = title;
    this.Description = description;
  }
  public ContentId: number;
  public Title: string;
  public Description: string;
}
