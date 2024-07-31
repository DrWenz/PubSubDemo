'use client';

import {usePubSubHub} from "@/app/PubSubHubProvider";
import {useEffect, useState} from "react";
import {NodeItem} from "@/app/interfaces";
import {node} from "prop-types";

function padToTwoDigits(number: number) {
  return number.toString().padStart(2, '0');
}

function padToThreeDigits(number: number) {
  return number.toString().padStart(3, '0');
}

function getFullDate(date: Date) {
  const day = padToTwoDigits(date.getDate());
  const month = padToTwoDigits(date.getMonth() + 1); // Monate sind nullbasiert
  const year = date.getFullYear();
  const hours = padToTwoDigits(date.getHours());
  const minutes = padToTwoDigits(date.getMinutes());
  const seconds = padToTwoDigits(date.getSeconds());
  const milliseconds = padToThreeDigits(date.getMilliseconds());

  return `${day}.${month}.${year} ${hours}:${minutes}:${seconds}.${milliseconds}`;
}

export default function Home() {
  const [nodes, setNodes] = useState<Array<NodeItem>>([]);
  const hub = usePubSubHub();

  useEffect(() => {
    const handleNodeUpdate = (node: NodeItem) => {
      setNodes((prev) => {
        const index = prev.findIndex((n) => n.name === node.name);
        if (index === -1) {
          return [...prev, node];
        } else {
          const updatedNodes = [...prev];
          updatedNodes[index] = node;
          return updatedNodes;
        }
      });
    };

    hub.registerForNodeUpdate(handleNodeUpdate);

    return () => {
      hub.unregisterForNodeUpdate(handleNodeUpdate);
    };
  }, [hub]);

  return (
      <main className="flex min-h-screen flex-col p-4">
        <div className="px-4 sm:px-6 lg:px-8">
          <div className="sm:flex sm:items-center">
            <div className="sm:flex-auto">
              <h1 className="text-base font-semibold leading-6 text-gray-900">PubSub Web Demo</h1>
              <p className="mt-2 text-sm text-gray-700">
                A Web demo for PubSub using SignalR.
              </p>
            </div>
          </div>
          <div className="mt-8 flow-root">
            <div className="-mx-4 -my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
              <div className="inline-block min-w-full py-2 align-middle sm:px-6 lg:px-8">
                <table className="min-w-full divide-y divide-gray-300">
                  <thead>
                  <tr>
                    <th scope="col" className="py-3.5 pl-4 pr-3 text-left text-sm font-semibold text-gray-900 sm:pl-0 w-56">
                      Name
                    </th>
                    <th scope="col" className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900 w-56">
                      Value
                    </th>
                    <th scope="col" className="px-3 py-3.5 text-left text-sm font-semibold text-gray-900 w-56">
                      LastUpdate
                    </th>
                  </tr>
                  </thead>
                  <tbody className="divide-y divide-gray-200">
                  {nodes.map((node: NodeItem, index: number) => (
                      <tr key={index}>
                        <td className="whitespace-nowrap py-4 pl-4 pr-3 text-sm font-medium text-gray-900 sm:pl-0  w-56">
                          {node.name}
                        </td>
                        <td className="whitespace-nowrap px-3 py-4 text-sm text-gray-500 w-56">{node.value}</td>
                        <td className="whitespace-nowrap px-3 py-4 text-sm text-gray-500 w-56">{getFullDate(new Date(node.lastUpdated))}</td>
                      </tr>
                  ))}
                  </tbody>
                </table>
              </div>
            </div>
          </div>
        </div>
      </main>
  );
}
