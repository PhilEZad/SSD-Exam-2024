import { HttpInterceptorFn } from '@angular/common/http';
import {environment} from '../../environments/environment';

export const backendUrlInterceptor: HttpInterceptorFn = (req, next) => {

    const url = environment.apiUrl + req.url;
    return next(req.clone({ url }));

};
