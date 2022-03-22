import { storeBuilder } from "dependingState";
//
export const pageAStoreBuilder = storeBuilder("PageAStore");
export const doSomething = pageAStoreBuilder.createAction("DoSomething");
//
