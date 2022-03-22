import { storeBuilder } from "dependingState";
export const appUIStoreBuilder = storeBuilder("AppUIStore");
export const loadData = appUIStoreBuilder.createAction("loadData");
