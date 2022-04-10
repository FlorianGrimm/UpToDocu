import {
    DSObjectStore,
    DSUIProps,
    DSUIViewStateBase,
    bindUIComponent
} from "dependingState";
import React from "react";

// import { countUp } from "./GraphLinkActions";

import { getAppStoreManager } from "~/singletonAppStoreManager";
//import OtherView from "../Other/OtherView";
import { GraphLinkValue } from "./GraphLinkValue";

type GraphLinkViewProps = {
} & DSUIProps<GraphLinkValue>;

type GraphLinkViewState = {
} & DSUIViewStateBase;

/**
 * create a new GraphLinkView
 * @param props stateValue.getViewProps()
 */
export function graphLinkView(props:GraphLinkViewProps): React.ReactNode{
    return React.createElement(GraphLinkView, props)
}

export default class GraphLinkView extends React.Component<GraphLinkViewProps, GraphLinkViewState>{
    constructor(props: GraphLinkViewProps) {
        super(props);
        this.state = bindUIComponent(this, props)
            .bindHandleAll()
            .setComponentWillUnmount()
            .getState();
    }

    handleAddClick() {
        getAppStoreManager().process("handleAddClick", ()=>{});
        // countUp.emitEvent(undefined);
    }

    // render(): React.ReactNode {
    //     const storeManager = getAppStoreManager();
    //     const projectStore = storeManager.projectStore;
    //     storeManager.process("handleAddClickAdd",() => {
    //         //dsLog.group("handleAddClick - Adding");
    //         for (let i = 0; i < 1000; i++) {
    //             const n = projectStore.entities.size + 1;
    //             projectStore.set({ ProjectId: n.toString(), ProjectName: `Name - ${n}` });
    //         }
    //     });
    // }

    render(): React.ReactNode {
        const viewProps = this.props.getRenderProps();
        const language = (viewProps.appState?.language) || "";

        return (<div>
            App
            <div>
                language:{language} - StateVersion: {this.props.getStateVersion()} - dt:{(new Date()).toISOString()}
            </div>
            <div>
                <button onClick={this.handleAddClickAdd}>add</button>
            </div>
            { /* otherUIStateValue && React.createElement(OtherView, otherUIStateValue.getViewProps()) */ }
        </div>);
    }
}