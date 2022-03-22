import { storeBuilder, } from 'dependingState';
export const navigatorBuilder = storeBuilder("navigator");
export const navigatorSetLocation = navigatorBuilder.createAction("setLocation");
