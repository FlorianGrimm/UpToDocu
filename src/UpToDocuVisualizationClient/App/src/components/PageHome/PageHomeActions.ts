import { storeBuilder } from "dependingState";

// the import type prevents that webpack add a depenency
import type { PageHomeStore } from "./PageHomeStore";

export const pageHomeStoreBuilder = storeBuilder<PageHomeStore['storeName']>("PageHomeStore");
// export const countDown = pageHomeStoreBuilder.createAction<undefined>("countDown");
// export const countUp = pageHomeStoreBuilder.createAction<undefined>("countUp");

/*
copy this to index.ts - main() - // create all stores

const pageHomeStore = new PageHomeStore(new PageHomeValue());
pageHomeStore,

copy this to AppStoreManager.ts

import type { PageHomeStore } from "~/components/PageHome/PageHomeStore";
pageHomeStore: PageHomeStore;
public pageHomeStore: PageHomeStore,
        this.attach(pageHomeStore);
*/