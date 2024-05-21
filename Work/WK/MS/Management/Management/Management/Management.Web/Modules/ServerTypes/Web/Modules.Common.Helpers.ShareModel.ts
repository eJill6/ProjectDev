import { ServiceRequest } from "@serenity-is/corelib";

export interface ShareModel extends ServiceRequest {
    Key?: string;
    Value?: string;
    Type?: string;
}
