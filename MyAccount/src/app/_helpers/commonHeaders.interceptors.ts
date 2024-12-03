import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable()
export class CommonHeader implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const authToken = sessionStorage.getItem("jwtToken");
        if (authToken) {
           // debugger
            req = req.clone({
                setHeaders: {
                    Authorization: 'bearer ' + authToken,
                }
            })
        }

        return next.handle(req);
    }
}