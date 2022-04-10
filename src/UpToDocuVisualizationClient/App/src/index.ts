import React, { Fragment } from 'react';
import ReactDom from 'react-dom';

import {
    dsLog, DSStateValue,
} from 'dependingState';

import { AppStoreManager } from './services/AppStoreManager';
import { setAppStoreManager } from './singletonAppStoreManager';
import { createBrowserHistory, DSRouterStore, DSRouterValue, getDSRouterValueInitial } from 'dependingStateRouter';
import { AppUIValue } from '~/components/AppUI/AppUIValue';
import { AppUIStore } from '~/components/AppUI/AppUIStore';
import { NavigatorStore } from '~/components/Navigator/NavigatorStore';
import { NavigatorValue } from '~/components/Navigator/NavigatorValue';
import { PageHomeStore } from '~/components/PageHome/PageHomeStore';
import { PageGraphStore } from '~/components/PageGraph/PageGraphStore';
import { GraphNavigatorStore } from '~/components/GraphNavigator/GraphNavigatorStore';
import { GraphNodeStore } from '~/components/GraphNode/GraphNodeStore';
import { GraphLinkStore } from '~/components/GraphLink/GraphLinkStore';

import AppUIView from '~/components/AppUI/AppUIView';
import { GraphNodeValue } from './components/GraphNode/GraphNodeValue';

function main() {
     // initialize log
     dsLog.initialize();

     // remove this if going productive
     dsLog.setEnabled();
 
     if (dsLog.enabled){
         dsLog.info("Abc main()");
     }
 
     // create all stores
    const routerStore = new DSRouterStore<DSRouterValue>(createBrowserHistory(), getDSRouterValueInitial());
    const navigatorStore = new NavigatorStore(new DSStateValue<NavigatorValue>({ page: "home", pathArguments: {} }));
    navigatorStore.setRouter(routerStore);
    const appUIStore = new AppUIStore(new AppUIValue());
    const pageHomeStore = new PageHomeStore(undefined);
    const pageGraphStore = new PageGraphStore(undefined);
    const graphNavigatorStore = new GraphNavigatorStore(undefined);
    const graphNodeStore = new GraphNodeStore();
    const graphLinkStore = new GraphLinkStore();

    // create appStoreManager
    const appStoreManager = new AppStoreManager(
        routerStore,
        navigatorStore,
        appUIStore,
        pageHomeStore,
        pageGraphStore,
        graphNavigatorStore,
        graphNodeStore,
        graphLinkStore
    );
    setAppStoreManager(appStoreManager);
    dsLog.attach(appStoreManager);
    appStoreManager.initialize();
    graphNodeStore.set(new GraphNodeValue("a",10,10));
    graphNodeStore.set(new GraphNodeValue("b",100,10));
    graphNodeStore.set(new GraphNodeValue("c",100,600));

    // start React
    const rootElement = React.createElement(
        AppUIView,
        appStoreManager.appUIStore.stateValue.getViewProps()
    );
    const appRootElement = window.document.getElementById("appRoot");
    if (appRootElement) {
        ReactDom.render(rootElement, appRootElement);
    } else {
        console.error("'appRoot' not defined.");
    }
}
try {
    main();
} catch (err) {
    console.error("Error while app boots.", err);
}
