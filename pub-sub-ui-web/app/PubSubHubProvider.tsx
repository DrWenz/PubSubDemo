'use client';

import React, {createContext, ReactNode, useCallback, useContext, useEffect, useRef, useState} from 'react';
import * as signalR from '@microsoft/signalr';
import {HubConnection} from '@microsoft/signalr';
import {HubConnectionState, NodeItem, PubSubHub} from "@/app/interfaces";

const PubSubHubContext = createContext<PubSubHub | null>(null);

export const PubSubHubProvider = ({
                                      children
                                  }: Readonly<{
    children: ReactNode;
}>) => {
    const [hubConnection, setHubConnection] = useState<HubConnection | null>(null);
    const [hubState, setHubState] = useState(HubConnectionState.Disconnected);
    const actionHandlers = useRef<Array<(node: NodeItem) => void>>([]);

    useEffect(() => {
        if (hubState === HubConnectionState.Connecting || hubState === HubConnectionState.Connected) {
            return;
        }

        setHubState(HubConnectionState.Connecting);
        const connect = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5201/api/v1/hub")
            .withAutomaticReconnect()
            .build();

        connect
            .start()
            .then(() => {
                setHubState(HubConnectionState.Connected);
                setHubConnection(connect);

                connect.on("NoteUpdated", (message) => {
                    if (actionHandlers.current.length > 0) {
                        actionHandlers.current.forEach((action: (node: NodeItem) => void) => {
                            action(message.value);
                        });
                    }
                });
            })
            .catch(err => {
                setHubState(HubConnectionState.Disconnected);
                console.error('Error while starting connection: ' + err);
            });
    }, [hubState]);

    const registerForNodeUpdate = useCallback((callback: (node: NodeItem) => void) => {
        if (actionHandlers.current.indexOf(callback) === -1)
            actionHandlers.current.push(callback);
    }, []);

    const unregisterForNodeUpdate = useCallback((callback: (node: NodeItem) => void) => {
        actionHandlers.current = actionHandlers.current.filter(handler => handler !== callback);
    }, []);

    return (
        <PubSubHubContext.Provider value={{hubState, registerForNodeUpdate, unregisterForNodeUpdate}}>
            {children}
        </PubSubHubContext.Provider>
    );
};

export const usePubSubHub = (): PubSubHub => {
    const context = useContext(PubSubHubContext);
    if (context === null) {
        throw new Error('useAppHub must be used within a AppHubProvider');
    }
    return context;
};
