import { storeBuilder } from "dependingState";

// the import type prevents that webpack add a depenency
import type { PageGraphStore } from "./PageGraphStore";

export const pageGraphStoreBuilder = storeBuilder<PageGraphStore['storeName']>("PageGraphStore");
// export const countDown = pageGraphStoreBuilder.createAction<undefined>("countDown");
// export const countUp = pageGraphStoreBuilder.createAction<undefined>("countUp");
