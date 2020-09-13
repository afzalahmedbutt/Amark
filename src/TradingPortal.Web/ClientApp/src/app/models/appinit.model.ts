import { TopicViewModel } from './topic/topic-view.model';
import { SpotPricePreviewViewModel, UpdateSpotsViewModel } from './WebSpotPrice/web-spot-prices.model';
import {SelectListItem } from './selectlistitem.model';

export class AppInitData {
  public UpdateSpotsViewModel: UpdateSpotsViewModel;
  public ServiceContract: TopicViewModel;
  public ServerTime: any;
  public MaxProductQuantity: number;
  public ContactFormData: ContactFormData;
  public IsPortalClosed: boolean;
}

export class ContactFormData {
  public EmailAliases: SelectListItem[];
  public ContactMethods: SelectListItem[];
}

export class ContactUsMessage {
  public Id: number = 0;
  public FirstName: string;
  public LastName: string;
  public SendToEmailAliasCode: string = "";
  public Phone: string;
  public Email: string;
  public PreferredContactMethod: string = "";
  
  public Message: string;
}


