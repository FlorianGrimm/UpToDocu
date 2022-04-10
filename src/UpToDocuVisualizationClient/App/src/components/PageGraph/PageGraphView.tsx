import {
    DSUIProps,
    DSUIViewStateBase,
    bindUIComponent
} from "dependingState";

import React from "react";

import "./PageGraphView.css"

//import { countUp } from "./PageGraphActions";

import { getAppStoreManager } from "~/singletonAppStoreManager";
//import OtherView from "../Other/OtherView";
import type { PageGraphValue } from "./PageGraphValue";

type PageGraphViewProps = {
} & DSUIProps<PageGraphValue>;

type PageGraphViewState = {
} & DSUIViewStateBase;

/**
 * create a new PageGraphView
 * @param props stateValue.getViewProps()
 */
export function pageGraphView(props: PageGraphViewProps): React.ReactNode {
    return React.createElement(PageGraphView, props)
}
export default class PageGraphView extends React.Component<PageGraphViewProps, PageGraphViewState>{
    constructor(props: PageGraphViewProps) {
        super(props);
        this.state = bindUIComponent(this, props)
            .add("stateVersion1", getAppStoreManager().graphNavigatorStore.stateValue.getViewProps())
            .add("stateVersion2", getAppStoreManager().graphNodeStore.stateValue.getViewProps())
            .bindHandleAll()
            .setComponentWillUnmount()
            .getState();
    }

    handleAddClick() {
        getAppStoreManager().process("handleAddClick", () => { });
    }

    handleNodeClick(e: React.MouseEvent<SVGRectElement>) {
    }

    render(): React.ReactNode {
        const renderProps = this.props.getRenderProps();
        
        /* otherUIStateValue && React.createElement(OtherView, otherUIStateValue.getViewProps()) */ 

        return (<div className="PageGraphView">
            <header>
                <button onClick={this.handleAddClick}>add</button>
            </header>
            <nav>
                xxxxxxxxxxx
                <ol>
                    <li>aaa</li>
                    <li>bbb</li>
                    <li>ccc</li>
                    <li>ddd</li>
                </ol>
            </nav>
            <main>
                <svg
                    xmlns="https://www.w3.org/2000/svg"
                    viewBox="0 0 1000 1000"
                >
                    <defs>
                        <marker id="triangle" viewBox="0 0 10 10"
                            refX="1" refY="5"
                            markerUnits="strokeWidth"
                            markerWidth="10" markerHeight="10"
                            orient="auto">
                            <path d="M 0 0 L 10 5 L 0 10 z" fill="#f00" />
                        </marker>
                    </defs>
                    <g transform="translate(100 100) scale(2 2)">

                        <desc>Red rectangle shape</desc>
                        <rect x="10" y="10" width="100" height="30" stroke="black" fill="transparent" onClick={this.handleNodeClick} />
                        <rect x="50" y="100" width="100" height="30" stroke="black" fill="transparent" />
                        <path d="m 55 40 l 0 20 q 0 10 10 10 l 40 0 q 10 0 10 10 l 0 10" markerEnd="url(#triangle)" stroke="black" fill="transparent" />
                    </g>
                </svg>
            </main>

            <footer>
                footer
            </footer>



        </div>);
    }
}