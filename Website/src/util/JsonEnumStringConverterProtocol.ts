import  * as signalR from '@microsoft/signalr';
import {HubMessage, ILogger} from "@microsoft/signalr";
export class JsonEnumStringConverterProtocol extends signalR.JsonHubProtocol{

    parseMessages(input: string, logger: ILogger): HubMessage[] {
        console.log("input");
        return super.parseMessages(input, logger);
    }
}