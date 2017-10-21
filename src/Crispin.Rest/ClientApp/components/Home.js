import React, { Component } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import Dashboard from  '../Dashboard'

const toggles = [
    { name: "one", description: "the first" },
    { name: "two", description: "the second" },
    { name: "three", description: "the third" }
]

export default class Home extends Component {
    render() {
        return <div>
            <Dashboard toggles={toggles} />
        </div>;
    }
}
