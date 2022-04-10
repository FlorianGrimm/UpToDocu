import { storeBuilder } from "dependingState";

// the import type prevents that webpack add a depenency
import type { GraphLinkStore } from "./GraphLinkStore";

export const graphLinkStoreBuilder = storeBuilder<GraphLinkStore['storeName']>("GraphLinkStore");
// export const countDown = graphLinkStoreBuilder.createAction<undefined>("countDown");
// export const countUp = graphLinkStoreBuilder.createAction<undefined>("countUp");

/*
copy this to index.ts - main() - // create all stores


graphLinkStore,

copy this to AppStoreManager.ts

import type { GraphLinkStore } from "~/components/GraphLink/GraphLinkStore";
graphLinkStore: GraphLinkStore;
public graphLinkStore: GraphLinkStore,
        this.attach(graphLinkStore);
*/