import {
    DSUIProps,
    DSUIViewStateBase,
    bindUIComponent
} from "dependingState";

import React from "react";

//import { countUp } from "./PageHomeActions";

import { getAppStoreManager } from "~/singletonAppStoreManager";
//import OtherView from "../Other/OtherView";
import type { PageHomeValue } from "./PageHomeValue";

import { routerPush } from "dependingStateRouter";

type PageHomeViewProps = {
} & DSUIProps<PageHomeValue>;

type PageHomeViewState = {
} & DSUIViewStateBase;

/**
 * create a new PageHomeView
 * @param props stateValue.getViewProps()
 */
export function pageHomeView(props:PageHomeViewProps): React.ReactNode{
    return React.createElement(PageHomeView, props)
}
export default class PageHomeView extends React.Component<PageHomeViewProps, PageHomeViewState>{
    constructor(props: PageHomeViewProps) {
        super(props);
        this.state = bindUIComponent(this, props)
            .bindHandleAll()
            .setComponentWillUnmount()
            .getState();
    }

    handleAddClick() {
        // getAppStoreManager().process("handleAddClick", ()=>{});
        // countUp.emitEvent(undefined);
    }

    
    handleClickGraph() {
        routerPush.emitEventAndProcess("to/Graph", { to: getAppStoreManager().navigatorStore.path.graph });
    }

    render(): React.ReactNode {
        const renderProps = this.props.getRenderProps();

        return (<div>
            App
            <div>
                PageHome - StateVersion: {this.props.getStateVersion()} - dt:{(new Date()).toISOString()}
            </div>
            <div>
                <button onClick={this.handleClickGraph}>Graph</button>
            </div>
            
        </div>);
    }
}