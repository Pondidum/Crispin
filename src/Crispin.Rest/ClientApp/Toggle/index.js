import React from 'react'


// Toggle NAME - Description
// Default: On, 3 other settings...
// User1: Off
// Group2: Off

const Toggle = ({ name, description }) => (
    <div className="toggle panel panel-default">
        <h3>{name}</h3>
        <span>{description}</span>
    </div>
)

export default Toggle
