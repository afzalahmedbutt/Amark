import { Directive,HostListener,ElementRef,Renderer2 } from '@angular/core';
//import { ETIMEDOUT } from 'constants';

@Directive({
  selector: '[appMousePosition]'
})
export class MousePositionDirective {

  constructor(
    private el: ElementRef,
    private renderer: Renderer2
  ) { }
  @HostListener('mouseover', ['$event']) onMouseOver(ev: any) {
   
    

    var xOffset = 285;
    var yOffset = 30;
    const p = this.renderer.createElement('p');
    this.renderer.setAttribute(p,'id', 'preview');
    const img = this.renderer.createElement('img');
    this.renderer.setAttribute(img, 'src', 'assets/images/Coins/Details/17b-zoom.png');
    this.renderer.appendChild(p, img);
    this.renderer.appendChild(document.body, p);
    var preview = document.getElementById('preview');
    this.renderer.setStyle(preview, 'top', (ev.clientY - xOffset));
    this.renderer.setStyle(preview, 'left', (ev.clientX - yOffset));

    //$("body").append("<p id='preview'><img src='" + "assets/images/Coins/Details/17b-zoom.png" + "' alt='Image preview' />" + "" + "</p>");
    //$("#preview")
    //  .css("top", (ev.clientY - xOffset) + "px")
    //  .css("left", (ev.clientX + yOffset) + "px");
    //  //.fadeIn("fast");
  





    //let part = this.el.nativeElement.querySelector('.card-text')
    //this.renderer.setElementStyle(part, 'display', 'block');
  }
}
