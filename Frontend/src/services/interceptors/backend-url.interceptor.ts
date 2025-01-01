import { HttpInterceptorFn } from '@angular/common/http';
import {environment} from '../../environments/environment';

export const backendUrlInterceptor: HttpInterceptorFn = (req, next) => {

    const url = environment.apiUrl + req.url;
    const clone = req.clone({ url });
    console.log('Backend URL Interceptor', clone);
    console.log('Backend URL Interceptor', url);
    console.log('Backend URL Interceptor', clone.url);
    console.log('Backend URL Interceptor ORIGINAL URL', req.url);
    return next(req.clone({ url }));

};
