
import { Injectable } from '@angular/core';

@Injectable()
export class DBkeys {

  public static readonly CURRENT_USER = "current_user";
  public static readonly USER_PERMISSIONS = "user_permissions";
  public static readonly ACCESS_TOKEN = "access_token";
  public static readonly ID_TOKEN = "id_token";
  public static readonly REFRESH_TOKEN = "refresh_token";
  public static readonly TOKEN_EXPIRES_IN = "expires_in";

  public static readonly REMEMBER_ME = "remember_me";


  public static readonly LANGUAGE = "language";
  public static readonly HOME_URL = "home_url";
  public static readonly THEME = "theme";
  public static readonly SHOW_DASHBOARD_STATISTICS = "show_dashboard_statistics";
  public static readonly SHOW_DASHBOARD_NOTIFICATIONS = "show_dashboard_notifications";
  public static readonly SHOW_DASHBOARD_TODO = "show_dashboard_todo";
  public static readonly SHOW_DASHBOARD_BANNER = "show_dashboard_banner";
}

@Injectable()
export class SessionKeys {
  public static readonly TRADING_ACTIVITY_OPTION = "trading_activity_option";
  public static readonly IS_CONTRACT_ACCEPTED = "is_contract_accepted";
  public static readonly IS_PAGE_REFRESH = "is_page_refresh";
  public static readonly ORDER_MODEL = "order_model";
  public static readonly IS_REVIEW_PAGE_REFRESH = "is_review_page_refresh";
}

@Injectable()
export class RoleKeys {
  public static readonly ADMIN = "Administrators";
  public static readonly FORUM_MODERATORS = "Forum Moderators";
  public static readonly REGISTERED = "Registered";
  public static readonly GUESTS = "Guests";
  public static readonly VENDORS = "Vendors";
  public static readonly ONLINE_TRADING = "Online Trading";
  public static readonly MTS = "MTS";
}
