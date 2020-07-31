import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environments/environment';

@Injectable()
export class AppConfig {
  static settings: IAppConfig;
  constructor(private http: HttpClient) { }
  load(): Promise<IAppConfig> {
    const jsonFile = `assets/config.${environment.name}.json`;
    return new Promise<IAppConfig>((resolve, reject) => {
      this.http.get(jsonFile)
        .toPromise()
        .then((response: IAppConfig) => {
          AppConfig.settings = <IAppConfig>response;
          resolve(<IAppConfig>response);
        })
        .catch((response: any) => {
          console.log(response);
          reject(`Could not load file '${jsonFile}': ${JSON.stringify(response)}`);
        });
    });
  }
}

export interface IAppConfig {
  API_BASE_URL: string;
}
