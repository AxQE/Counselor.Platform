import { GET } from "./base.service.client";

export const getAllTransports = (onlyActive = true) => {
    return GET(`transports?onlyActive${onlyActive}`);
}

export const getTransport = (id) => {
    return GET(`transports/${id}`);
}

export const getTransportCommand = (id) => {
    return GET(`transports/${id}/commands`);
}