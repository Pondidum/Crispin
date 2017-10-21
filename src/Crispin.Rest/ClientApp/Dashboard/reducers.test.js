import reducer from './reducers'

it('should return the default state when input is null', () => {
    const state = reducer(undefined, {});
    expect(state).toEqual({ loading: false, toggles: [] })
})

it('should return the current state when type is unknown', () => {
    const state = reducer({ test: true }, { type: "wat" });
    expect(state).toEqual({ test: true })
})

it('should set loading true when requesting all toggles', () => {
    const state = reducer({}, { type: "REQUEST_ALL_TOGGLES" });
    expect(state).toEqual({ loading: true, toggles: [] })
})

it('should populate toggles when receiving all toggles', () => {
    const state = reducer({}, {
        type: "RECEIVE_ALL_TOGGLES",
        toggles: [ 1, 2, 3 ]
    });

    expect(state).toEqual({
        loading: false,
        toggles: [ 1, 2, 3]
    })
})