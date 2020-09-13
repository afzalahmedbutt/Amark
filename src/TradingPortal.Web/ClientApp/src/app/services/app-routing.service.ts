import { Injectable } from "@angular/core";
import { Router, ActivatedRouteSnapshot } from "@angular/router";

@Injectable()
export class AppRoutingService {

  constructor(private router: Router) { }

  public getRouteTitle(): string {
    return this.getRouteData("title");
  }

  private getRouteData(data: string): any {
    const root = this.router.routerState.snapshot.root;
    var lastChild = this.lastChild(root);
    return lastChild.data[data];
    //return this.lastChild(root).data[0][data];
  }

  private lastChild(route: ActivatedRouteSnapshot): ActivatedRouteSnapshot {
    if (route.firstChild) {
      return this.lastChild(route.firstChild);
    } else {
      return route;
    }
  }
}
