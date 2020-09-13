
import { animate, state, style, transition, trigger } from '@angular/animations';



export const fadeInOut = trigger('fadeInOut', [
  transition(':enter', [style({ opacity: 0 }), animate('1s ease-in', style({ opacity: 1 }))]),
  transition(':leave', [animate('0.4s 10ms ease-out', style({ opacity: 0 }))])
])

export function fadeInOutTime(name : string,durationIn: number = 0.4,durationOut: number = 0.4) {
  return trigger(name, [
    transition(':enter', [style({ opacity: 0 }), animate(`${durationIn}s ease-in`, style({ opacity: 1 }))]),
    transition(':leave', [animate(`${durationOut }s 10ms ease-out`, style({ opacity: 0 }))])])
}





export function flyInOut(duration: number = 0.2) {
  return trigger('flyInOut', [
    state('in', style({ opacity: 1, transform: 'translateX(0)' })),
    transition('void => *', [style({ opacity: 0, transform: 'translateX(-100%)' }), animate(`${duration}s ease-in`)]),
    transition('* => void', [animate(`${duration}s 10ms ease-out`, style({ opacity: 0, transform: 'translateX(100%)' }))])
  ])
}

export function moveInOut(name: string, durationIn: number = 0.2, durationOut: number = 0.2) {
  return trigger(name, [
    state('in', style({ opacity: 1, transform: 'translateX(0)' })),
    transition('void => *', [style({ opacity: 0, transform: 'translateY(-20%)' }), animate(`${durationIn}s ease-in`)]),
    transition('* => void', [animate(`${durationOut}s 50ms ease-out`, style({ opacity: 0, transform: 'translateY(30%)' }))])
  ])
}

export function moveInOutDropShip(name: string, durationIn: number = 0.2, durationOut: number = 0.2) {
  return trigger(name, [
    state('in', style({ opacity: 1, transform: 'translateX(0)' })),
    transition('void => *', [style({ opacity: 0, transform: 'translateY(-35%)' }), animate(`${durationIn}s ease-in`)]),
    transition('* => void', [animate(`${durationOut}s 50ms ease-out`, style({ opacity: 0, transform: 'translateY(-100%)' }))])
  ])
}

export function moveIn(name: string, duration: number = 0.2) {
  return trigger(name, [
    state('in', style({ opacity: 1, transform: 'translateX(0)' })),
    transition('void => *', [style({ opacity: 0, transform: 'translateY(-20%)' }), animate(`${duration}s ease-in`)])
    
  ])
}

export function fadeOut(name:string,duration : number = 0.2) {
  return trigger(name, [
    transition(':leave', [animate(`${duration}s 10ms ease-out`, style({ opacity: 0 }))])
  ])
}
