import { ServiceRequest } from "@serenity-is/corelib";

export interface LoginRequest extends ServiceRequest {
    Token?: string;
}
