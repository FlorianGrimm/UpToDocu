import { storeBuilder } from "dependingState";
//
export const pageBStoreBuilder = storeBuilder("PageBStore");
export const doSomething = pageBStoreBuilder.createAction("DoSomething");
//
