import React from 'react'


// Toggle NAME - Description
// Default: On, 3 other settings...
// User1: Off
// Group2: Off

const toggleState = (state) => state === 1 ? 'On' : 'Off';

const Toggle = ({ toggle }) => {
    const { name, description, state } = toggle;

    const specificStates =
        Object.keys(state.users).length +
        Object.keys(state.groups).length;

    return (
        <div className="toggle panel panel-default col-sm-12">
            <h3>{name}</h3>
            <span>{description}</span>
            <p>Default: {toggleState(state.anonymous)}{specificStates > 0 ? ` and ${specificStates} other states.`: '.'}</p>
        </div>
    )
}

export default Toggle
