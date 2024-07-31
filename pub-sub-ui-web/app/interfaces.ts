export interface NodeItem {
    name:string;
    value: any;
    lastUpdated: string;
}

export enum HubConnectionState {
    Connecting,
    Connected,
    Disconnected
}

export enum HubMessageType {
    NoteUpdated = "NoteUpdated",
}

export interface PubSubHub {
    hubState: HubConnectionState;
    registerForNodeUpdate: (callback: (node: any) => void) => void;
    unregisterForNodeUpdate: (callback: (node: any) => void) => void;
}